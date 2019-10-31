using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingScheduler
{
    public enum SyncState
    {
        Synced,
        Unsynced,
        Updated,
        Deleted
    }
    public class Training
    {
        public long id = -1;
        public long coachId;
        public string name;
        public List<long> traineesId = new List<long>();
        public DateTime date;
        public SyncState syncState = SyncState.Unsynced;
    }
}
