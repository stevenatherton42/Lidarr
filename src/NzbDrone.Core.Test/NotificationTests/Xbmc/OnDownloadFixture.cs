using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using Moq;
using NUnit.Framework;
using NzbDrone.Core.MediaFiles;
using NzbDrone.Core.Notifications;
using NzbDrone.Core.Notifications.Xbmc;
using NzbDrone.Core.Test.Framework;
using NzbDrone.Core.Music;

namespace NzbDrone.Core.Test.NotificationTests.Xbmc
{
    [TestFixture]
    public class OnDownloadFixture : CoreTest<Notifications.Xbmc.Xbmc>
    {
        private DownloadMessage _downloadMessage;
        
        [SetUp]
        public void Setup()
        {
            var artist = Builder<Artist>.CreateNew()
                                        .Build();

            var trackFile = Builder<TrackFile>.CreateNew()
                                                   .Build();

            _downloadMessage = Builder<DownloadMessage>.CreateNew()
                                                       .With(d => d.Artist = artist)
                                                       .With(d => d.TrackFile = trackFile)
                                                       .With(d => d.OldFiles = new List<TrackFile>())
                                                       .Build();

            Subject.Definition = new NotificationDefinition();
            Subject.Definition.Settings = new XbmcSettings
                                          {
                                              UpdateLibrary = true
                                          };
        }

        private void GivenOldFiles()
        {
            _downloadMessage.OldFiles = Builder<TrackFile>.CreateListOfSize(1)
                                                            .Build()
                                                            .ToList();

            Subject.Definition.Settings = new XbmcSettings
                                          {
                                              UpdateLibrary = true,
                                              CleanLibrary = true
                                          };
        }

        [Test]
        public void should_not_clean_if_no_episode_was_replaced()
        {
            Subject.OnDownload(_downloadMessage);

            Mocker.GetMock<IXbmcService>().Verify(v => v.Clean(It.IsAny<XbmcSettings>()), Times.Never());
        }

        [Test]
        public void should_clean_if_episode_was_replaced()
        {
            GivenOldFiles();
            Subject.OnDownload(_downloadMessage);

            Mocker.GetMock<IXbmcService>().Verify(v => v.Clean(It.IsAny<XbmcSettings>()), Times.Once());
        }
    }
}
