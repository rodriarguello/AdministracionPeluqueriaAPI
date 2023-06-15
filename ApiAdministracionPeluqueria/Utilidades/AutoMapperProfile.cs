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
using Microsoft.IdentityModel.Tokens;

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
            CreateMap<Mascota, MascotaSinClienteDTO>()
                .ForMember(mascotaDTO=> mascotaDTO.Enfermedades, opciones=> opciones.MapFrom(MapMascotaEnfermedadesAenfermedadesDTO))
                .ForMember(mascotaDTO=>mascotaDTO.IdEnfermedades, opciones => opciones.MapFrom(MapIdEnfermedades));
            CreateMap<Mascota, MascotaNombreFechaNacimientoDTO>().ReverseMap();
            CreateMap<MascotaModificarDTO,Mascota>();


            #endregion



            #region FECHAS

            CreateMap<Fecha,FechaSinCalendarioDTO>();

            #endregion




            #region HORARIOS
            CreateMap<Horario, HorarioSinCalendarioDTO>().ReverseMap();
            #endregion


        }

        private List<EnfermedadDTO> MapMascotaEnfermedadesAenfermedadesDTO(Mascota mascota, MascotaSinClienteDTO mascotaSinClienteDTO)
        {
            var resultado = new List<EnfermedadDTO>(); 

            if(mascota.Enfermedades.Count < 1) return resultado;

            foreach (var enfermedad in mascota.Enfermedades)
            {
                
                resultado.Add(new EnfermedadDTO
                {
                    Id = enfermedad.EnfermedadId,
                    Nombre = enfermedad.Enfermedad.Nombre
                });
            }


            return resultado;

        }

        private List<int> MapIdEnfermedades(Mascota mascota, MascotaSinClienteDTO mascotaSinClienteDTO)
        {
            var respuesta = new List<int>();
            if (mascota.Enfermedades.Count < 1) return respuesta;

            foreach (var enfermedad in mascota.Enfermedades)
            {

                respuesta.Add(enfermedad.EnfermedadId);

            }

            return respuesta;


        }
    }
}
