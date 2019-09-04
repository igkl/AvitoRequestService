using System;

namespace avitoRequestService
{


    struct Product
    {

        public int Id { get; set; }

        public string IdFromAvito { get; set; }
        public string Link { get; set; }

        //readonly string photoLink;
        public string Name { get; set; }
        public string Price { get; set; }
        public string Position { get; set; }

        public DateTime DataTime { get; set; }

        public string DataType { get; set; }

        public string Age { get; set; }


    }
}