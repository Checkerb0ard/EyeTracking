using BoneLib.Notifications;
using Il2CppSLZ.Marrow.Warehouse;
using LabFusion.Downloading;
using LabFusion.Downloading.ModIO;
using MelonLoader;

namespace EyeTracking.Fusion;

public static class ContentDownloader
{
    private const int SyncerId = 5232917;
    //private const string SyncerBarcode = "EyeTrackingBL.EyeTracking.Spawnable.EyeTrackingRB";

    private enum DownloadStatus
    {
        Unverified,
        Attempted,
        Success
    }

    private static DownloadStatus _status = DownloadStatus.Unverified;
    
    public static void PreloadSyncer(Action? whenReady=null)
    {
        switch (_status)
        {
            case DownloadStatus.Success:
                whenReady?.Invoke();
                break;
            case DownloadStatus.Attempted:
                return;
            case DownloadStatus.Unverified:
            {
                _status = DownloadStatus.Attempted;
                // For now, just re-download the spawnable each game load when it's needed by an eye-tracked avatar in Fusion,
                // it's under 30kb, will stay up-to-date this way, and avoids issues with manual installers not having the
                // manifest files required for Fusion's auto downloader (which defeats the point of this feature).

                // if (AssetWarehouse.Instance.TryGetCrate(barcode, out _))
                // {
                //     _status = DownloadStatus.Success;
                //     whenReady?.Invoke();
                //     return;
                // }

                ModIODownloader.EnqueueDownload(new ModTransaction
                {
                    ModFile = new ModIOFile(SyncerId),
                    Callback = info =>
                    {
                        if (info.result == ModResult.SUCCEEDED)
                        {
                            MelonLogger.Msg("Eye tracking content updated");
                            whenReady?.Invoke();
                            _status = DownloadStatus.Success;
                        }
                        else
                        {
                            Notifier.Send(new Notification
                            {
                                Title = "Download Failure",
                                Message = "Eye Tracking content failed to update! Are you logged into Mod.io through Bonelab?",
                                Type = NotificationType.Error
                            });
                        }
                    }
                });

                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(whenReady));
        }
    }
}