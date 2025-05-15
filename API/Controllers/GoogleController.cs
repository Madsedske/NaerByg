using API.Services;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoogleController : ControllerBase
    {
        private readonly IGoogleService _googleService;

        public GoogleController(IGoogleService googleService)
        {
            _googleService = googleService;
        }

        /// <summary>
        /// Sends a request to google api to calculate the distance between a user-provided address and a shop address 
        /// using the Google Distance Matrix API.
        /// </summary>
        /// <param name="inputAddress">Address provided by the user.</param>
        /// <param name="shopAddress">The shop address.</param>
        /// <param name="shopPostArea">Postal area to include in the calculation context.</param>
        /// <returns>
        /// An <see cref="ActionResult{GoogleDistanceResponse}"/> containing distance and duration details if successful,
        /// or a BadRequest result if the calculation fails.
        /// </returns>
        [HttpGet("calculate")]
        public async Task<ActionResult<GoogleDistanceResponse>> GetCalculatedDistance(string? inputAddress, string shopAddress, int shopPostArea)
        {

            if (string.IsNullOrWhiteSpace(inputAddress))
            {
                return Ok(new GoogleDistanceResponse
                {
                    status = "OK",
                    rows = new List<Row>
                    {
                        new Row
                        {
                            elements = new List<Element>
                            {
                                new Element
                                {
                                    distance = new DistanceInfo
                                    {
                                        text = "Ikke angivet",
                                        value = 0
                                    },
                                    duration = new DurationInfo
                                    {
                                        text = "Ikke angivet",
                                        value = 0
                                    },
                                    status = "NO_INPUT_ADDRESS"
                                }
                            }
                        }
                    }
                });
            }
            var data = await _googleService.CalculateDistance(inputAddress, shopAddress, shopPostArea);

            if (data == null)
                return BadRequest($"Failed to fetch data");

            return Ok(data);

        }
    }

}
