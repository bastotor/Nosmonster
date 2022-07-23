﻿using OpenNos.DAL.Interface.PropertiesMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenNos.Mapper.Props
{
    public abstract class ModuleMapper<TDTOEntity, TEntity> : IModuleMapper<TDTOEntity, TEntity>
    {
        public Type GetTypeDto() => typeof(TDTOEntity);
        public Type GetTypeEntity() => typeof(TEntity);

        public abstract bool ToDTO(TEntity input, TDTOEntity output);
        public abstract bool ToEntity(TDTOEntity input, TEntity output);
    }
}
