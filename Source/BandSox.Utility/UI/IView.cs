namespace BandSox.Utility.UI
{
	public interface IView
	{
		ViewModel.Base ViewModelObject { get; }
	}

	public interface IView<T> : IView where T : ViewModel.Base
	{
		T ViewModel { get; set; }
	}
}