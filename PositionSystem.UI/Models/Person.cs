using System;

namespace PositionSystem.UI.Models
{
    public class Person 
    {
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                
            }
        }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime DateTimeAdded { get; set; }

    }

    public class Manager : Person
    {

    }
}
