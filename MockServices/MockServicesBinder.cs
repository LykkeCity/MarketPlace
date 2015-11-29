using Common.IocContainer;
using Core.Clients;
using MockServices.Clients;

namespace MockServices
{
    public static class MockServicesBinder
    {
        public static void BindMockServices(this IoC ioc)
        {
            ioc.RegisterSingleTone<ISrvSmsConfirmator, SrvSmsConfirmatorMock>();

        }
    }
}
