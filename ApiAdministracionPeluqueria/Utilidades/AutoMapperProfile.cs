using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.CalendarioDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.ClienteDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.EnfermedadDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.MascotaDTO;
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


            // Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InJvZHJpYXJndWVsbG85NkBnbWFpbC5jb20iLCJleHAiOjE2NzgyMTEzMDZ9.s3836B6o6MAqSbwfSd6d9c8mZxTOkYNYxihhjbjrLGw
            #region TURNOS


            CreateMap<TurnoModificarDTO, Turno>().ReverseMap();
            CreateMap<TurnoDTO, Turno>().ReverseMap();

            #endregion


            #region CALENDARIOS


            CreateMap<Calendario,CalendarioDTO>().ReverseMap();


            #endregion


            #region MASCOTAS

            CreateMap<MascotaCreacionDTO,Mascota>();
            CreateMap<MascotaDTO, Mascota>().ReverseMap();


            #endregion

        }
    }
}
