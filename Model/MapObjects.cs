using AutoMapper;
using System.Collections.Generic;

namespace DC
{
    public class CreateObject
    {
        public T Factory<T>() where T : new()
        {
            return new T();
        }
    }
    public class MapObjects<Ts, Td>
    {        
        public void Copy(ref Ts source, ref Td desti)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Ts, Td>());
            var mapper = config.CreateMapper();
            desti = mapper.Map<Td>(source);
        }
        public void Copy(List<Ts> source, List<Td> desti)
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Ts, Td>());
                var mapper = config.CreateMapper(); 
                desti = mapper.Map(source, desti);                
            }
            catch (System.Exception ex)
            {

                var v1 = ex.Message;
            } 
        }         
    }
}
