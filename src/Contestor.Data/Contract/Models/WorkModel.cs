namespace Contestor.Proto.Data
{
    public class WorkModel : BaseModel
    {
        public long ParticipantId { get; set; }

        public long ContestId { get; set; }

        public int RoundNumber { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}
