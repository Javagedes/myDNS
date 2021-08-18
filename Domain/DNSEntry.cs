using System;

namespace Domain
{
    /*
     A simplified DNS entry row
     Id       -> Id in the table for lookups
     HostName -> The URL
     TTL      -> How long the entry can sit in cache before needing to be updated with the database
     Type     -> The Type of Record, Currently Supports A (iPV4) and AAAA (iPV6)
     Value    -> The value corresponding to the Type
    */
    public class DNSEntry
    {   
        public int    Id        { get; set; }
        public string HostName  { get; set; }

        public int    TTL       { get; set; }

        public string Type      { get; set; }

        public string Value     { get; set; }

    }
}
