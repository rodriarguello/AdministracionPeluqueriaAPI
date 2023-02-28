using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.ClienteDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.EnfermedadDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.RazaDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO;
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


            #region ENFERMEDADES


            CreateMap<EnfermedadCreacionDTO, Enfermedad>();
            CreateMap<EnfermedadDTO, Enfermedad>().ReverseMap();

            #endregion


            #region RAZAS


            CreateMap<RazaCreacionDTO, Raza>();
            CreateMap<RazaDTO, Raza>().ReverseMap();

            #endregion


            #region TURNOS


            CreateMap<TurnoModificarDTO, Turno>();
            CreateMap<TurnoDTO, Turno>().ReverseMap();

            #endregion

        }
    }
}
