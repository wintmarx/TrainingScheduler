using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingScheduler
{
    public class Training
    {
        public long id;
        public User coach;
        public string name;
        public List<User> trainees;
        public DateTime date;
        public bool isDeleted = false;
    }
}
