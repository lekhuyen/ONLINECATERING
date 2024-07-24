using APIRESPONSE.Models;
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
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "Error from service",
                        Data = null
                    });
                }
                // Send a confirmation email to the user
                await _emailService.SendEmailAsync(contact.Email, "Contact Received", "Your contact message has been received. We will get back to you soon.");
                contact.IsAdminResponse = false;
                await _contactRepositories.CreateContact(contact);

                

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "A message has been sent Successfully",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error from service",
                    Data = null
                });
            }
        }

        // POST: api/contact/respond/{id}
        [HttpPost("respond/{id}")]
        public async Task<IActionResult> RespondToMessage(string id, [FromBody] string response)
        {
            try
            {
                bool result = await _contactRepositories.RespondToMessage(id, response);

                if (!result)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "The message is not found",
                    });
                }

                var contact = await _contactRepositories.GetContactById(id);
                if (contact == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Status = 1,
                        Message = "The message is not found",
                    });
                }

                // Send the response email to the user
                await _emailService.SendEmailAsync(contact.Email, "Response to Your Contact", response);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Respond To The Message Successfully",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error from service",
                    Data = null
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
                    Message = "Get All Contact Successfully",
                    Data = contacts
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error from service",
                    Data = null
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
                        Message = "The contact is not found",
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Get Contact Successfully",
                    Data = contact
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error from service",
                    Data = null
                });
            }
        }

        // update client's contact
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateContact(string id, [FromBody] Contact contact)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(new ApiResponse
        //            {
        //                Success = false,
        //                Status = 1,
        //                Message = "Update Contact failed"
        //            });
        //        }

        //        bool result = await _contactRepositories.UpdateContact(id, contact);

        //        if (!result)
        //        {
        //            return NotFound(new ApiResponse
        //            {
        //                Success = false,
        //                Status = 1,
        //                Message = "Contact not found",
        //            });
        //        }

        //        return Ok(new ApiResponse
        //        {
        //            Success = true,
        //            Status = 0,
        //            Message = "Update Contact Successfully",
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ApiResponse
        //        {
        //            Success = false,
        //            Status = 1,
        //            Message = "Error from service",
        //            Data = null
        //        });
        //    }
        //}

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
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Status = 0,
                    Message = "Delete Contact Successfully",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Status = 1,
                    Message = "Error from service",
                    Data = null
                });
            }
        }
    }
}
