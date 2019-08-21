namespace ManjuuDomain.IDomain
{
    public interface IPingable
    {
          string IpAddresV4 { get;  }
          string TargetPort { get;  }
          string Remarks { get; }        
    }
}