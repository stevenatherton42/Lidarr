using System;
using System.Collections.Generic;
using System.Linq;
using NzbDrone.Core.MediaCover;
using NzbDrone.Core.Music;
using Lidarr.Api.V1.Albums;
using Lidarr.Http.REST;

namespace Lidarr.Api.V1.Artist
{
    public class ArtistResource : RestResource
    {
        //Todo: Sorters should be done completely on the client
        //Todo: Is there an easy way to keep IgnoreArticlesWhenSorting in sync between, Series, History, Missing?
        //Todo: We should get the entire Profile instead of ID and Name separately

        public ArtistStatusType Status { get; set; }

        public bool Ended => Status == ArtistStatusType.Ended;

        public DateTime? LastInfoSync { get; set; }

        public string ArtistName { get; set; }
        public string ForeignArtistId { get; set; }
        public string MBId { get; set; }
        public int TADBId { get; set; }
        public int DiscogsId { get; set; }
        public string AllMusicId { get; set; }
        public string Overview { get; set; }
        public string ArtistType { get; set; }
        public string Disambiguation { get; set; }
        public List<string> PrimaryAlbumTypes { get; set; }
        public List<string> SecondaryAlbumTypes { get; set; }
        public List<Links> Links { get; set; }

        public int? AlbumCount { get; set; }
        public int? TotalTrackCount { get; set; }
        public int? TrackCount { get; set; }
        public int? TrackFileCount { get; set; }
        public long? SizeOnDisk { get; set; }
        //public SeriesStatusType Status { get; set; }

        public List<MediaCover> Images { get; set; }
        public List<Member> Members { get; set; }

        public string RemotePoster { get; set; }
        public List<AlbumResource> Albums { get; set; }


        //View & Edit
        public string Path { get; set; }
        public int QualityProfileId { get; set; }
        public int LanguageProfileId { get; set; }

        //Editing Only
        public bool AlbumFolder { get; set; }
        public bool Monitored { get; set; }

        public string RootFolderPath { get; set; }
        //public string Certification { get; set; }
        public List<string> Genres { get; set; }
        public string CleanName { get; set; }
        public string SortName { get; set; }
        public HashSet<int> Tags { get; set; }
        public DateTime Added { get; set; }
        public AddArtistOptions AddOptions { get; set; }
        public Ratings Ratings { get; set; }
        public string NameSlug { get; set; }

        //TODO: Add series statistics as a property of the series (instead of individual properties)
    }

    public static class SeriesResourceMapper
    {
        public static ArtistResource ToResource(this NzbDrone.Core.Music.Artist model)
        {
            if (model == null) return null;

            return new ArtistResource
            {
                Id = model.Id,

                ArtistName = model.Name,
                //AlternateTitles
                SortName = model.SortName,

                Status = model.Status,
                Overview = model.Overview,
                ArtistType = model.ArtistType,
                Disambiguation = model.Disambiguation,

                PrimaryAlbumTypes = model.PrimaryAlbumTypes,
                SecondaryAlbumTypes = model.SecondaryAlbumTypes,

                Images = model.Images,

                Albums = model.Albums.ToResource(),
                //Year = model.Year,

                Path = model.Path,
                QualityProfileId = model.ProfileId,
                LanguageProfileId = model.LanguageProfileId,
                Links = model.Links,

                AlbumFolder = model.AlbumFolder,
                Monitored = model.Monitored,

                LastInfoSync = model.LastInfoSync,
                //SeriesType = model.SeriesType,
                CleanName = model.CleanName,
                ForeignArtistId = model.ForeignArtistId,
                NameSlug = model.NameSlug,
                RootFolderPath = model.RootFolderPath,
                //Certification = model.Certification,
                Genres = model.Genres,
                Tags = model.Tags,
                Added = model.Added,
                AddOptions = model.AddOptions,
                Ratings = model.Ratings
            };
        }

        public static NzbDrone.Core.Music.Artist ToModel(this ArtistResource resource)
        {
            if (resource == null) return null;

            return new NzbDrone.Core.Music.Artist
            {
                Id = resource.Id,

                Name = resource.ArtistName,
                //AlternateTitles
                SortName = resource.SortName,

                Status = resource.Status,
                Overview = resource.Overview,
                //NextAiring
                //PreviousAiring
                // Network = resource.Network,
                //AirTime = resource.AirTime,
                Images = resource.Images,

                //Albums = resource.Albums.ToModel(),
                //Year = resource.Year,

                Path = resource.Path,
                ProfileId = resource.QualityProfileId,
                LanguageProfileId = resource.LanguageProfileId,
                Links = resource.Links,
                PrimaryAlbumTypes = resource.PrimaryAlbumTypes,
                SecondaryAlbumTypes = resource.SecondaryAlbumTypes,

                AlbumFolder = resource.AlbumFolder,
                Monitored = resource.Monitored,

                LastInfoSync = resource.LastInfoSync,
                //SeriesType = resource.SeriesType,
                CleanName = resource.CleanName,
                ForeignArtistId = resource.ForeignArtistId,
                NameSlug = resource.NameSlug,
                RootFolderPath = resource.RootFolderPath,
                //Certification = resource.Certification,
                Genres = resource.Genres,
                Tags = resource.Tags,
                Added = resource.Added,
                AddOptions = resource.AddOptions,
                Ratings = resource.Ratings
            };
        }

        public static NzbDrone.Core.Music.Artist ToModel(this ArtistResource resource, NzbDrone.Core.Music.Artist artist)
        {
            var updatedArtist = resource.ToModel();

            artist.ApplyChanges(updatedArtist);

            return artist;
        }

        public static List<ArtistResource> ToResource(this IEnumerable<NzbDrone.Core.Music.Artist> artist)
        {
            return artist.Select(ToResource).ToList();
        }

        public static List<NzbDrone.Core.Music.Artist> ToModel(this IEnumerable<ArtistResource> resources)
        {
            return resources.Select(ToModel).ToList();
        }
    }
}
