using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.CalendarioDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.ClienteDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.EnfermedadDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.FechaDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.HorarioDTO;
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
            CreateMap<Cliente, ClienteSinMascotasDTO>().ReverseMap();

            CreateMap<Cliente,ClienteModificarDTO>().ReverseMap();

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


            CreateMap<TurnoModificarDTO, Turno>().ReverseMap();
            CreateMap<TurnoDTO, Turno>().ReverseMap();
            

            #endregion


            #region CALENDARIOS


            CreateMap<Calendario,CalendarioDTO>().ReverseMap();


            #endregion


            #region MASCOTAS

            CreateMap<MascotaCreacionDTO,Mascota>();
            CreateMap<MascotaSinCliente, Mascota>().ReverseMap();
            CreateMap<Mascota, MascotaSinClienteDTO>().ReverseMap();


            #endregion



            #region FECHAS

            CreateMap<Fecha,FechaSinCalendarioDTO>();

            #endregion




            #region HORARIOS
            CreateMap<Horario, HorarioSinCalendarioDTO>().ReverseMap();
            #endregion
        }
    }
}
