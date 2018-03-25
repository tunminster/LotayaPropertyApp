using LotayaPropertyApp.Models;
using System.Collections.Generic;

namespace LotayaPropertyApp.Services
{
    public interface ILotayaApiService
    {
        List<PropertyFeedModel> GetPropertyFeedList();
    }
}