import PropTypes from 'prop-types';
import React, { Component } from 'react';
import { createSelector } from 'reselect';
import connectSection from 'Store/connectSection';
import { fetchReleases, setReleasesSort, grabRelease } from 'Store/Actions/releaseActions';
import createClientSideCollectionSelector from 'Store/Selectors/createClientSideCollectionSelector';
import createUISettingsSelector from 'Store/Selectors/createUISettingsSelector';
import InteractiveEpisodeSearch from './InteractiveEpisodeSearch';

function createMapStateToProps() {
  return createSelector(
    createClientSideCollectionSelector(),
    createUISettingsSelector(),
    (releases, uiSettings) => {
      return {
        longDateFormat: uiSettings.longDateFormat,
        timeFormat: uiSettings.timeFormat,
        ...releases
      };
    }
  );
}

const mapDispatchToProps = {
  fetchReleases,
  setReleasesSort,
  grabRelease
};

class InteractiveEpisodeSearchConnector extends Component {

  //
  // Lifecycle

  componentDidMount() {
    const {
      albumId,
      isPopulated
    } = this.props;

    // If search results are not yet isPopulated fetch them,
    // otherwise re-show the existing props.

    if (!isPopulated) {
      this.props.fetchReleases({
        albumId
      });
    }
  }

  //
  // Listeners

  onSortPress = (sortKey, sortDirection) => {
    this.props.setReleasesSort({ sortKey, sortDirection });
  }

  onGrabPress = (guid, indexerId) => {
    this.props.grabRelease({ guid, indexerId });
  }

  //
  // Render

  render() {
    return (
      <InteractiveEpisodeSearch
        {...this.props}
        onSortPress={this.onSortPress}
        onGrabPress={this.onGrabPress}
      />
    );
  }
}

InteractiveEpisodeSearchConnector.propTypes = {
  albumId: PropTypes.number.isRequired,
  isPopulated: PropTypes.bool.isRequired,
  fetchReleases: PropTypes.func.isRequired,
  setReleasesSort: PropTypes.func.isRequired,
  grabRelease: PropTypes.func.isRequired
};

export default connectSection(
  createMapStateToProps,
  mapDispatchToProps,
  undefined,
  undefined,
  { section: 'releases' }
)(InteractiveEpisodeSearchConnector);
