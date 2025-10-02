using System.Net;
using System.Text.Json;
using Labb3TravelBookingAPI.Data;
using Labb3TravelBookingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Labb3TravelBookingAPI.Functions
{
    public class BookingFunctions
    {
        [Function("CreateBooking")]
        public async Task<HttpResponseData> CreateBooking(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "bookings")] HttpRequestData req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var booking = JsonSerializer.Deserialize<Booking>(requestBody);
            booking.Id = Guid.NewGuid().ToString();
            BookingRepository.Add(booking);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(booking);
            return response;
        }

        [Function("GetBookings")]
        public HttpResponseData GetBookings(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "bookings")] HttpRequestData req)
        {
            var bookings = BookingRepository.GetAll();
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(bookings);
            return response;
        }

        [Function("GetBookingById")]
        public HttpResponseData GetBookingById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "bookings/{id}")] HttpRequestData req, string id)
        {
            var booking = BookingRepository.GetById(id);
            var response = req.CreateResponse();

            if (booking is null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                response.StatusCode = HttpStatusCode.OK;
                response.WriteAsJsonAsync(booking);
            }

            return response;
        }

        [Function("UpdateBooking")]
        public async Task<HttpResponseData> UpdateBooking(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "bookings/{id}")] HttpRequestData req, string id)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updatedBooking = JsonSerializer.Deserialize<Booking>(requestBody);
            updatedBooking.Id = id;

            var existing = BookingRepository.GetById(id);
            var response = req.CreateResponse();

            if (existing is null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                BookingRepository.Update(updatedBooking);
                response.StatusCode = HttpStatusCode.OK;
                await response.WriteAsJsonAsync(updatedBooking);
            }

            return response;
        }

        [Function("DeleteBooking")]
        public HttpResponseData DeleteBooking(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "bookings/{id}")] HttpRequestData req, string id)
        {
            var existing = BookingRepository.GetById(id);
            var response = req.CreateResponse();

            if (existing is null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                BookingRepository.Delete(id);
                response.StatusCode = HttpStatusCode.OK;
            }

            return response;
        }
    }
}
