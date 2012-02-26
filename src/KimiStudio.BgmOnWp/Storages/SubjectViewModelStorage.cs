using Caliburn.Micro;
using KimiStudio.BgmOnWp.ViewModels;

namespace KimiStudio.BgmOnWp.Storages
{
    public class SubjectViewModelStorage : StorageHandler<SubjectViewModel>
    {
        public override void Configure()
        {
            Id(x => x.Id);
            Property(x => x.Id).InPhoneState().RestoreAfterActivation();
        }
    }
}