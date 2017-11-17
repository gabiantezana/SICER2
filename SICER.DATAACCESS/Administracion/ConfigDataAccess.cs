using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using PagedList;
using SICER.EXCEPTION;
using SICER.HELPER;
using SICER.MODEL;
using SICER.VIEWMODEL.Administracion.Config;

namespace SICER.DATAACCESS.Administracion
{
    public class ConfigDataAccess
    {
        private DataContext DataContext { get; set; }
        public ConfigDataAccess(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public IPagedList<CONFIG> LstConfigs(String CampoBuscar, Int32? page)
        {
            var query = DataContext.Context.CONFIG.AsQueryable();
            var p = page ?? 1;
            if (!string.IsNullOrEmpty(CampoBuscar))
            {
                foreach (var token in CampoBuscar.Split(' '))
                {
                    query = query.Where(x => x.descripcion.Contains(token));
                }
            }
            return query.OrderBy(x => x.Id).ToPagedList(p, query.Count());
        }

        public IPagedList<CONFIG> LstConfigs(Int32? page)
        {
            var query = DataContext.Context.CONFIG.AsQueryable();
            var p = page ?? 1;
            return query.OrderBy(x => x.Id).ToPagedList(p, !query.Any()? 1: query.Count());
        }

        public List<JsonEntityTwoString> GetConfigs(String CampoBuscar)
        {
            var query = DataContext.Context.CONFIG.AsQueryable();

            if (!string.IsNullOrEmpty(CampoBuscar))
            {
                foreach (var token in CampoBuscar.Split(' '))
                {
                    query = query.Where(x => x.descripcion.Contains(token));
                }
            }

            return query.Select(x => new JsonEntityTwoString
            {
                id = x.Id,
                text = x.descripcion
            }).ToList();
        }

        public IPagedList<CONFIG> GetConfigs(String ConfigId, Int32? p)
        {
            var query = DataContext.Context.CONFIG.AsQueryable();
            var page = p ?? 1;

            if (!String.IsNullOrEmpty(ConfigId))
                query = query.Where(x => x.descripcion == ConfigId);

            return query.OrderBy(a => a.descripcion).ToPagedList(page, query.Count());
        }

        public ConfigViewModel GetConfig(string ConfigId)
        {
            CONFIG config = DataContext.Context.CONFIG.FirstOrDefault(x => x.Id == ConfigId);
            ConfigViewModel model = new ConfigViewModel();

            if (config != null)
            {
                model.id = config.Id;
                model.valor = config.Valor;
                model.descripcion = config.descripcion;


            }

            return model;
        }

        public String GetCONFIGValue(String idConfig)
        {
            String valor = null;

            var data = DataContext.Context.CONFIG.Where(x => x.Id == idConfig).FirstOrDefault();
            var data2 = DataContext.Context.CONFIG.ToList();
            if (data != null)
                return valor = data.Valor;
            else
                throw new Exception("No se encontró el valor " + idConfig + " en las configuraciones");
        }

        public String SaveConfig(ConfigViewModel model)
        {

            using (var transaction = new TransactionScope())
            {
                try
                {
                    CONFIG config = DataContext.Context.CONFIG.FirstOrDefault(x => x.Id == model.id);


                    bool existe = true;

                    if (config == null)
                    {
                        config = new CONFIG();
                        existe = false;
                    }

                    bool configrepetido = false;
                    if (!existe)
                        configrepetido = ValidateConfigRepetido(model);

                    if (configrepetido)
                    {
                        throw new CustomException(new TempDataEntityException { Mensaje = "El parámetro ya se encuentra registrado", TipoMensaje = MessageTypeException.Warning }, DataContext);
                    }
                    config.Id = model.id;
                    config.descripcion = model.descripcion;
                    config.Valor = model.valor;

                    if (!existe)
                        DataContext.Context.CONFIG.Add(config);

                    DataContext.Context.SaveChanges();
                    transaction.Complete();

                    return config.Id;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public bool ValidateConfigRepetido(ConfigViewModel model)
        {
            bool repetido = false;

            var config = DataContext.Context.CONFIG.FirstOrDefault(x => x.Id.ToUpper() == model.id.ToUpper());

            if (config != null)
                repetido = true;

            return repetido;

        }

        public void FillListsFromOtherModel(ConfigViewModel fromModel, ref ConfigViewModel model)
        {
            model.id = fromModel.id;
            model.valor = fromModel.valor;
            model.descripcion = fromModel.descripcion;

        }
    }
}
