namespace PFA_Gestion_Laureats.Models
{
    public class Test
    {
        public int Id { get; set; }

        public DateTime Date_Test { get; set; }
        public DateTime Heure_Test { get; set; }
        public string Description { get; set; }
        public int EntrepriseId { get; set; }
        public Entreprise entreprise { get; set; }
        public int AgentDirectionId { get; set; }
        public AgentDirection agentDirection { get; set; }

    }
}
