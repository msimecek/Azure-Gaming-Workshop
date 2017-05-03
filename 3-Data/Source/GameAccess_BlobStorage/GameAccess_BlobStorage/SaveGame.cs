using System;
using System.Collections.Generic;
using System.Text;

namespace GameAccess_BlobStorage
{
    public class SaveGame
    {
        public int CharId { get; set; }
        public string CharName { get; set; }
        public int Score { get; set; }

        public string Serialize()
        {
            return $"{CharId};{CharName};{Score}";
        }

    }
}
