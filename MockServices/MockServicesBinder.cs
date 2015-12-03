using Common.IocContainer;
using Core.BitCoin;
using Core.Clients;
using MockServices.BitCoin;
using MockServices.Clients;

namespace MockServices
{
    public static class MockServicesBinder
    {
        public static void BindMockServices(this IoC ioc)
        {
            ioc.RegisterSingleTone<ISrvSmsConfirmator, SrvSmsConfirmatorMock>();
            ioc.RegisterSingleTone<ISrvLykkeWallet, SrvLykkeWalletMock>();
        }
    }
}
