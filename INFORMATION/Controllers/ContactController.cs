using APIRESPONSE.Models;
using INFORMATION.API.Models;
using INFORMATION.API.Services;
using INFORMATIONAPI.Models;
using INFORMATIONAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace INFORMATIONAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactRepositories _contactRepositories;
        private readonly EmailService _emailService;

        public ContactController(IContactRepositories contactRepositories, EmailService emailService)
        {
            _contactRepositories = contactRepositories;
            _emailService = emailService;
        }

        // POST: api/contact
        [HttpPost]
        public async Task<IActionResult> CreateContact([FromBody] Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Invalid contact data",
                    Data = null
                });
            }

            try
            {
                contact.IsAdminResponse = false;
                await _contactRepositories.CreateContact(contact);

                // Send a confirmation email to the user
                await _emailService.SendEmailAsync(
                    contact.Email,
                    "Contact Received",
                    "Your contact message has been received. We will get back to you soon.");

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Contact message sent successfully"
                });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "An error occurred while processing your request",
                    Data = ex.Message
                });
            }
        }

        // POST: api/contact/respond/{id}
        [HttpPost("respond/{id}")]
        public async Task<IActionResult> RespondToMessage(string id, [FromBody] string response)
        {
            if (string.IsNullOrWhiteSpace(response))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Response content cannot be empty",
                    Data = null
                });
            }

            try
            {
                var contact = await _contactRepositories.GetContactById(id);
                if (contact == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Contact message not found",
                        Data = null
                    });
                }

                bool result = await _contactRepositories.RespondToMessage(id, response);
                if (!result)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Failed to update response",
                        Data = null
                    });
                }

                // Send the response email to the user
                await _emailService.SendEmailAsync(
                    contact.Email,
                    "Response to Your Contact",
                    response);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Response sent successfully"
                });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "An error occurred while processing your request",
                    Data = ex.Message
                });
            }
        }

        // GET: api/contact
        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            try
            {
                var contacts = await _contactRepositories.GetAllContacts();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Contacts retrieved successfully",
                    Data = contacts
                });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "An error occurred while retrieving contacts",
                    Data = ex.Message
                });
            }
        }

        // GET: api/contact/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(string id)
        {
            try
            {
                var contact = await _contactRepositories.GetContactById(id);
                if (contact == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Contact not found",
                        Data = null
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Contact retrieved successfully",
                    Data = contact
                });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "An error occurred while retrieving the contact",
                    Data = ex.Message
                });
            }
        }

        // DELETE: api/contact/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(string id)
        {
            try
            {
                bool result = await _contactRepositories.DeleteContact(id);

                if (!result)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Contact not found",
                        Data = null
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Contact deleted successfully"
                });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "An error occurred while deleting the contact",
                    Data = ex.Message
                });
            }
        }

        // POST: api/contact/subscribe
        // POST: api/contact/subscribe
        [HttpPost("subscribe")]
        public async Task<IActionResult> SubscribeNewsletter([FromBody] EmailRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Email))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Email address cannot be empty",
                    Data = null
                });
            }

            try
            {
                // Send a confirmation email to the user
                await _emailService.SendEmailAsync(
                    request.Email,
                    "Thank You for Subscribing",
                    "Thank you for subscribing! We will be in touch with you shortly and ensure that you receive all the latest updates and news as soon as they're available. We appreciate your interest and look forward to keeping you informed with the most current information and exciting updates. Stay tuned!");

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Subscription successful. A confirmation email has been sent."
                });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "An error occurred while processing your subscription",
                    Data = ex.Message
                });
            }
        }

    }
}
