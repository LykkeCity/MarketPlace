﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Assets
{

    public interface IAsset
    {
        string Id { get; }
        string Name { get; }
    }

    public class Asset : IAsset
    {
        public string Id { get; set; }
        public string Name { get; set; }


        public static Asset Create(string id, string name)
        {
            return new Asset
            {
                Id = id,
                Name = name
            };
        }

        public static Asset CreateDefault()
        {
            return new Asset
            {

            };
        }
       

    }
    

    public interface IAssetsRepository
    {
        Task RegisterAssetAsync(IAsset asset);
        Task EditAssetAsync(string id, IAsset asset);
        Task<IEnumerable<IAsset>> GetAssetsAsync();
        Task<IAsset> GetAssetAsync(string id);
    }

}
