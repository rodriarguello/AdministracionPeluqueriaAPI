using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.CalendarioDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.ClienteDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.EnfermedadDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.MascotaDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.RazaDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.UsuarioDTO;
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
            CreateMap<Turno, TurnoSinMascotaDTO>().ReverseMap();
            

            #endregion


            #region CALENDARIOS


            CreateMap<Calendario,CalendarioDTO>().ReverseMap();


            #endregion


            #region MASCOTAS

            CreateMap<MascotaCreacionDTO,Mascota>();
            CreateMap<Mascota, MascotaDTO>()
                .ForMember(mascotaDTO => mascotaDTO.Enfermedades, opciones => opciones.MapFrom(Map_MascotaEnfermedades_EnfermedadesDTO))
                .ForMember(mascotaDTO => mascotaDTO.IdEnfermedades, opciones => opciones.MapFrom(MapIdEnfermedades))
                .ForMember(mascotaDTO=> mascotaDTO.Alergias, opciones=> opciones.MapFrom(Map_MascotaAlergias_AlergiasDTO))
                .ForMember(mascotaDTO=> mascotaDTO.IdAlergias, opciones=> opciones.MapFrom(MapIdAlergias));
            
            CreateMap<Mascota, MascotaNombreFechaNacimientoDTO>().ReverseMap();
            
            CreateMap<MascotaModificarDTO,Mascota>();


            #endregion


            #region USUARIOS


            CreateMap<Usuario, UsuarioDTO>();

            #endregion


        }


        private List<EnfermedadDTO> Map_MascotaEnfermedades_EnfermedadesDTO(Mascota mascota, MascotaDTO mascotaSinClienteDTO)
        {
            var resultado = new List<EnfermedadDTO>(); 

            if(mascota.MascotaEnfermedades.Count < 1) return resultado;

            foreach (var enfermedad in mascota.MascotaEnfermedades)
            {
                
                resultado.Add(new EnfermedadDTO
                {
                    Id = enfermedad.IdEnfermedad,
                    Nombre = enfermedad.Enfermedad.Nombre
                });
            }


            return resultado;

        }

        private List<int> MapIdEnfermedades(Mascota mascota, MascotaDTO mascotaSinClienteDTO)
        {
            var respuesta = new List<int>();
            if (mascota.MascotaEnfermedades.Count < 1) return respuesta;

            foreach (var enfermedad in mascota.MascotaEnfermedades)
            {

                respuesta.Add(enfermedad.IdEnfermedad);

            }

            return respuesta;


        }

        private List<AlergiaDTO> Map_MascotaAlergias_AlergiasDTO(Mascota mascota, MascotaDTO mascotaSinClienteDTO)
        {
            var resultado = new List<AlergiaDTO>();

            if (mascota.MascotaAlergias.Count < 1) return resultado;

            foreach (var alergia in mascota.MascotaAlergias)
            {

                resultado.Add(new AlergiaDTO
                {
                    Id = alergia.IdAlergia,
                    Nombre = alergia.Alergia.Nombre
                });
            }


            return resultado;

        }

        private List<int> MapIdAlergias(Mascota mascota, MascotaDTO mascotaSinClienteDTO)
        {
            var respuesta = new List<int>();

            if (mascota.MascotaAlergias.Count < 1) return respuesta;

            foreach (var alergias in mascota.MascotaAlergias)
            {

                respuesta.Add(alergias.IdAlergia);

            }

            return respuesta;


        }

    }
}
