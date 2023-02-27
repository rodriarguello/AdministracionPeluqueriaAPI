using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.ClienteDTO;
using AutoMapper;

namespace ApiAdministracionPeluqueria.Utilidades
{
    public class AutoMapperProfile: Profile
    {

        public AutoMapperProfile()
        {

            #region CLIENTES
            CreateMap<ClienteCreacionDTO, Cliente>();
            CreateMap<Cliente, ClienteDTO>().ReverseMap();

            #endregion

            #region ALERGIAS


            CreateMap<AlergiaCreacionDTO,Alergia>();
            CreateMap<AlergiaDTO,Alergia>().ReverseMap();

            #endregion

        }
    }
}
