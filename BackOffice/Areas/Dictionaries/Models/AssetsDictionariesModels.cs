using System.Collections.Generic;
using BackOffice.Models;
using Core.Assets;

namespace BackOffice.Areas.Dictionaries.Models
{
    public class AssetsIndexViewModel
    {
        public IEnumerable<IAsset> Assets { get; set; }
    }

    public class EditAssetDialogViewModel : IPersonalAreaDialog
    {
        public string Caption { get; set; }
        public string Width { get; set; }

        public IAsset Asset { get; set; }
    }


    public class EditAssetModel : IAsset
    {
        public string EditId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}