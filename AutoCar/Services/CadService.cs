using AutoCar.Entities;
using AutoMapper;
using System.Buffers.Text;
using System.Xml;

namespace AutoCar.Services
{
    public interface ICadService
    {
        public Dictionary<string, string> Search(string s);
    }

    public class CadService : ICadService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public CadService(ApplicationDbContext context, IMapper mapper, IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public Dictionary<string, string> Search(string ss)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(ss);
            string s = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

            s = s.Replace("WINDOWS-1252", "UTF-8");

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(s);

            XmlNodeList attr;
            XmlNodeList ster;

            attr = xmldoc.GetElementsByTagName("UML:Attribute");
            ster = xmldoc.GetElementsByTagName("UML:Stereotype");

            Dictionary<string, string> dic = new Dictionary<string, string>();

            for (int i = 0; i < attr.Count; i++)
            {
                Console.WriteLine(ster[i + 1].Attributes.GetNamedItem("name").Value + ": " + attr[i].Attributes.GetNamedItem("name").Value);
                dic[ster[i + 1].Attributes.GetNamedItem("name").Value] = attr[i].Attributes.GetNamedItem("name").Value;
            }

            return dic;
        }
    }
}
