using System.Collections.Generic;
using System.Linq;
using MessageBoard.Data.Models;

namespace MessageBoard.Data.Repositories
{
    public class MessageRepository
    {
        private static List<Message> messages = new List<Message>();
        private static int nextId = 1;

        public IEnumerable<Message> GetAll()
        {
            return messages;
        }

        public Message GetById(int id)
        {
            return messages.FirstOrDefault(m => m.Id == id);
        }

        public void Add(Message message)
        {
            message.Id = nextId++;
            messages.Add(message);
        }

        public void Update(Message message)
        {
            var index = messages.FindIndex(m => m.Id == message.Id);
            if (index >= 0)
            {
                messages[index] = message;
            }
        }

        public void Delete(int id)
        {
            var message = GetById(id);
            if (message != null)
            {
                messages.Remove(message);
            }
        }
    }
}
