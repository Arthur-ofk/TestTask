
import * as React from 'react';

import { Component } from 'react';
import { BlobServiceClient, AnonymousCredential } from '@azure/storage-blob';


class Form extends Component {

    constructor() {
        super();
        this.state = {
            selectedFile: null,
            email: '',
            fileError: '',
            emailError: '',
        };
    }
     
     handleUpload = async () => {
        if (!this.selectedFile) {
            alert('Please select a file to upload.');
            return;
        }

        // Replace with your Azure Blob Storage account settings
         const connectionString = 'DefaultEndpointsProtocol=https;AccountName=arturblobcontainers;AccountKey=JKkJ0/TTolH2xkIOjl617+fJujaLeWRszo4WVf5iAj6aLam22yZC6uug/WK+SWu8OGFCrFulVHvF+AStKaAbtg==;EndpointSuffix=core.windows.net';
         const accountName = 'arturblobcontainers';
         const accountKey = 'JKkJ0/TTolH2xkIOjl617+fJujaLeWRszo4WVf5iAj6aLam22yZC6uug/WK+SWu8OGFCrFulVHvF+AStKaAbtg==';
        const containerName = 'testtask';
        const blobName = this.selectedFile.name;

        
         const blobServiceClient = new BlobServiceClient(connectionString,new AnonymousCredential());
        const containerClient = blobServiceClient.getContainerClient(containerName);
        const blockBlobClient = containerClient.getBlockBlobClient(blobName);

        try {
            await blockBlobClient.uploadBrowserData(this.selectedFile);
            alert('File uploaded successfully.');
            this.electedFile(null);
        } catch (error) {
            console.error('Error uploading file:', error.message);
        }
    };
    handleFileChange = (event) => {
        const file = event.target.files[0];
        if (file && file.name.endsWith('.docx')) {
            this.setState({ selectedFile: file, fileError: '' });
        } else {
            this.setState({ selectedFile: null, fileError: 'Please select a .docx file.' });
        }
    };

    handleEmailChange = (event) => {
        const email = event.target.value;
        const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        if (emailPattern.test(email)) {
            this.setState({ email, emailError: '' });
        } else {
            this.setState({ emailError: 'Please enter a valid email address.' });
        }
    };

    handleSubmit = async (event) => {
        event.preventDefault();
        if (this.state.selectedFile && this.state.email) {
            try {
                const response = await fetch('/api/ProcessBlob?email=${encodeURIComponent(this.state.email)}&file=${encodeURIComponent(this.state.selectedFile)}', {
                    method: 'POST'
                });

                if (response.ok) {
                   
                    console.log('Email sent successfully');
                } else {
                    
                    console.error('Failed to send email');
                }
            } catch (error) {
                console.error('Error:', error);
            }
        } else {
            
            alert('Please fill in all fields correctly.');
        }
    };

    render() {
        return (
            <div>
                <h2>Upload a .docx File</h2>
                <form onSubmit={this.handleSubmit}>
                    <div>
                        <label>Upload .docx File:</label>
                        <input type="file" accept=".docx" onChange={this.handleFileChange} />
                        <div className="error">{this.state.fileError}</div>
                    </div>
                    <div>
                        <label>Email:</label>
                        <input type="text" value={this.state.email} onChange={this.handleEmailChange} />
                        <div className="error">{this.state.emailError}</div>
                    </div>
                    <button type="submit">Submit</button>
                </form>
            </div>
        );
    }
}

    //const Form = () => {

    //    return (
            
                
    //            <h1>11111
    //            </h1>
                    
                
                
    //    )

    //}
   export default Form;
