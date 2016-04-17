using AutoMapper;
using KeepItSolved.Models;
using KeepItSolved.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace KeepItSolved.Controllers.Api
{
	[Authorize]
	[Route("api/cards")]
    public class CardController : Controller
    {
		private ISolvedRepository _repository;

		public CardController(ISolvedRepository repository)
		{
			_repository = repository;
		}

		[HttpGet("")]
		public JsonResult Get()
		{
			var cards = _repository.GetUserFlashcards(User.Identity.Name);
			var cardsResult = Mapper.Map<IEnumerable<FlashcardViewModel>>(cards);

			return Json(cardsResult);
		}

		[HttpPost("")]
		public JsonResult Post([FromBody]FlashcardViewModel vm)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var newCard = Mapper.Map<Flashcard>(vm);
					newCard.UserName = User.Identity.Name;

					//Save to Database
					_repository.AddCard(newCard);

					if (_repository.SaveAll())
					{
					Response.StatusCode = (int)HttpStatusCode.Created;
					return Json(Mapper.Map<FlashcardViewModel>(newCard));
					}
				}
			}
			catch (Exception ex)
			{
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
				return Json(new { Message = ex.Message });
			}


			Response.StatusCode = (int)HttpStatusCode.BadRequest;
			return Json( new { Message = "Failed", ModelState = ModelState });
		}
    }
}
