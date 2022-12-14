namespace Test_2.ScheduleSetup
{
    public class GMRSchedule
    {
        public int ID { get; }
        public List<GMRClass> Classes { get; private set; }
        public DateTime Date { get; private set; }
        //ALOT MORE THEN JUST THIS
        public GMRSchedule(List<GMRClass> classes, DateTime date, int id)
        {
            Classes = classes;
            Date = date;
            ID = id;
        }

        public List<GMRClass> GetSessionByTime(int timeSlotID) => Classes.Where(x => x.TimeSlotID == timeSlotID).ToList();

        public (GMRClass gmrClass, Student student) GetClosestClass(string username, bool entered, DateTime time)
        {
            var getClass = Classes.Where(x => x.Students.Exists(s => s.Username == username));
            IOrderedEnumerable<GMRClass> classes;

            if(entered)
            {
                classes = getClass.Where(x => Communicator.timeSlotMap[x.TimeSlotID].end.TimeOfDay >= time.TimeOfDay).OrderBy(t => Communicator.timeSlotMap[t.TimeSlotID].end.TimeOfDay);
            }
            else
            {
                classes = getClass.Where(x => Communicator.timeSlotMap[x.TimeSlotID].start.TimeOfDay <= time.TimeOfDay).OrderBy(t => Communicator.timeSlotMap[t.TimeSlotID].end.TimeOfDay);
            }
            var cls = classes.First();
            var std = cls.Students.Where(x => x.Username == username).First();
            return (cls, std);
        }

    }
}
