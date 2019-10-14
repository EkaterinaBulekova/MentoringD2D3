import React, { Component } from 'react';
import {Button} from 'reactstrap';
import {UrlInput} from './UrlInput';

export class Downloader extends Component {
  static displayName = Downloader.name;

  constructor(props) {
    super(props);
    this.state = { downloads: [], started: [], loading: true };
  }

  componentDidMount() {
    this.populateDownloadsData();
  }

  onCancelClick = (download) => {
    this.cancelDownload(download);
    const started = this.state.started.filter(s => s !== download.id);
    this.setState({started: started});
  };

  onStartClick = (download) => {
    const downloads = this.state.downloads.map(d =>{if(d.id === download.id) d.content="Loading"; return d});
    const started = this.state.started;
    started.push(download.id);
    this.setState({started: started, downloads: downloads});
    this.startDownload(download);
  };

  onImputFormSubmit = (e) => {
    e.preventDefault();
    const url = e.target.site.value;
    this.createDownload(url);
  }

  renderDownloadsTable(downloads, started) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>URL</th>
            <th>Content</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {downloads.map(download =>
            <tr key={download.id}>
              <td>{download.site}</td>
              <td><iframe title={download.id} srcDoc={download.content}></iframe></td>
              <td>{
                started.includes(download.id)
                ?
                <Button color="primary" onClick={() => {this.onCancelClick(download)}}>Cancel</Button>
                :
                <Button color="primary" onClick={() => {this.onStartClick(download)}}>Start</Button>
              }
              </td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : this.renderDownloadsTable(this.state.downloads, this.state.started);

    return (
      <div>
        <h1 id="tabelLabel" >Downloader</h1>
        <p>This component downloads from urls.</p>
        <UrlInput onSubmit={this.onImputFormSubmit}/>
        {contents}
      </div>
    );
  }

  async populateDownloadsData() {
    const response = await fetch('download');
    const data = await response.json();
    this.setState({ downloads: data, loading: false });
  }

  async createDownload(url) {
    const response = await fetch('download', {
      method: 'PUT',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        Site: url, 
        Content: ""})
    });
    const data = await response.json();
    const downloads = this.state.downloads;
    downloads.unshift(data);

    this.setState({ downloads: downloads, loading: false });
  }

  async startDownload(download) {
    const response = await fetch('download', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        ID: download.id, 
        Site: download.site, 
        Content: download.content})
    });
    const data = await response.json();
    const downloads = this.state.downloads.map(d =>{if(d.id === download.id) return data; return d});

    this.setState({ downloads: downloads, loading: false });
  }

  async cancelDownload(download) {
    const response = await fetch('download', {
      method: 'DELETE',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        ID: download.id, 
        Site: download.site, 
        Content: download.content})
    });
    const data = await response.json();
    const downloads = this.state.downloads.map(d =>{if(d.id === download.id) return data; return d});

    this.setState({ downloads: downloads, loading: false });
  }
}
