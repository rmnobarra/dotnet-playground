using System.Collections.Generic;
using System.Web.Http;
using MessageBoard.Data.Models;
using MessageBoard.Data.Repositories;

namespace MessageBoard.API.Controllers
{
    [RoutePrefix("api/messages")]
    public class MessagesController : ApiController
    {
        private MessageRepository repository = new MessageRepository();

        // GET: api/messages
        [HttpGet, Route("")]
        public IEnumerable<Message> Get()
        {
            return repository.GetAll();
        }

        // GET: api/messages/{id}
        [HttpGet, Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var message = repository.GetById(id);
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }

        // POST: api/messages
        [HttpPost, Route("")]
        public IHttpActionResult Post([FromBody] Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            repository.Add(message);
            return CreatedAtRoute("DefaultApi", new { id = message.Id }, message);
        }

        // PUT: api/messages/{id}
        [HttpPut, Route("{id:int}")]
        public IHttpActionResult Put(int id, [FromBody] Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingMessage = repository.GetById(id);
            if (existingMessage == null)
            {
                return NotFound();
            }
            message.Id = id;
            repository.Update(message);
            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }

        // DELETE: api/messages/{id}
        [HttpDelete, Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var message = repository.GetById(id);
            if (message == null)
            {
                return NotFound();
            }
            repository.Delete(id);
            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }
    }
}
