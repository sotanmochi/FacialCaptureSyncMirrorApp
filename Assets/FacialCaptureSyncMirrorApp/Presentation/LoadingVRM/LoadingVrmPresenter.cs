namespace FacialCaptureSync.MirrorApp
{
    public sealed class LoadingVrmPresenter
    {
        readonly LoadingVrmView _view;
        readonly AvatarContext _context;

        public LoadingVrmPresenter
        (
            LoadingVrmView view,
            AvatarContext context
        )
        {
            _view = view;
            _context = context;
        }

        public void Initialize()
        {
            _view.OnOpenLocalFile += async(path) => 
            {
                await _context.LoadAvatarResourceAsync(path);
            };
        }
    }
}