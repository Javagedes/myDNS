export default class DnsEntry 
{
    id: string
    hostName: string 
    ttl: string
    type: string
    value: string

    constructor(id: string, hostName: string, ttl: string, type: string, value: string)
    {
        this.id = id;
        this.hostName = hostName;
        this.ttl = ttl;
        this.type = type;
        this.value = value;
    }
}