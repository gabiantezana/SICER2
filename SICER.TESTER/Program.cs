﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SICER.MODEL;

namespace SICER.TESTER
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {


                /*
                DataContext dataContext = new DataContext();
                dataContext.Context = new SICEREntities();
                dataContext.SAPDbServerType = SAPDbServerType.SQL;

                var sadf = dataContext.Context.SP_SAPProveedor().ToList();
                Console.ReadLine(); */

                using (var ctx = new SICEREntities())
                {
                    var rol = new Rol()
                    {
                        Codigo = "Superadmin",
                        Descripcion = "Super administrador"
                    };
                    ctx.Rol.Add(rol);

                    var usuario = new Usuario()
                    {
                        Nombres = "Gabriela",
                        Apellidos = "Antezana",
                        Correo = "correo@correo.com",
                        Documento = "12345678",
                        Telefono = "000000",
                        FechaNacimiento = DateTime.Now,
                        UserName = "gabiantezana",
                        RolId = 1,
                        Estado = true,
                    };
                    ctx.Usuario.Add(usuario);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                Console.Read();
                throw;
            }
        }
    }
}
