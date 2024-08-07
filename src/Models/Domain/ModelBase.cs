using System;


namespace Models.Domain{

    public class ModelBase{
        public int Id {get;set;}
        public bool isActive {get;set;} = true;
        public DateTime CreatedDate {get;set;} = new DateTime();
        public string CreatedBy {get;set;} = "TestUser";
        public DateTime? LastUpdated {get;set;}

    }


}