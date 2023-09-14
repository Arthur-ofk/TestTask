import React, { useState } from 'react';
import './style.css';




const Form = () => {
    const [selectedFile, setSelectedFile] = useState(null);
    const [email, setEmail] = useState('');
    const [fileError, setFileError] = useState('');
    const [emailError, setEmailError] = useState('');
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [uploadSuccess, setUploadSuccess] = useState(false);

    const handleFileChange = (event) => {
        const file = event.target.files[0];
        if (file && file.name.endsWith('.docx')) {
            setSelectedFile(file);
            setFileError('');
        } else {
           
            setFileError('Please select a .docx file.');
        }
    };

    const handleEmailChange = (event) => {
        setEmail(event.target.value);
        setEmailError('');
    };
    const handleFileClear = () => {
        setSelectedFile(null);
    }
    const handleSubmit = async (event) => {
        event.preventDefault();
        const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        if (!emailPattern.test(email)) {
            setEmailError('Please enter a valid email address.');
            setUploadSuccess(false);
            return;
        }
        const data = new FormData();
        data.append('file', selectedFile);
        data.append('email', email);

        try {
            if (selectedFile && email) {
                const response = await fetch('blob', {
                    method: 'POST',
                    body: data,
                });

                if (response.ok) {
                    console.log('File uploaded successfully.');
                    setSelectedFile(null);
                    setUploadSuccess(true);
                    setSelectedFile(null);
                    setEmail('');
                } else {
                    setSelectedFile(null);
                    setEmail('');
                    setUploadSuccess(false);
                    console.error('File upload failed.');
                }
            }
        } catch (error) {
            console.error('An error occurred during file upload:', error);
        }
    };
    
    return (
        
        
        <div className="fancy-form">
            <h2>Upload a .docx File</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <div className="selected-file">
                        {selectedFile ? selectedFile.name : 'No file selected'}
                    </div>
                    <label htmlFor="file-upload" className="file-upload-label">
                        <input
                            type="file"
                            accept=".docx"
                            id="file-upload"
                          
                            onChange={handleFileChange}
                            style={{ display: 'none' }}
                        />
                        Choose a .docx File
                    </label>
                    <div className="error">{fileError}</div>
                </div>
                
                <div>
                    <label htmlFor="email" className="email-label">
                        Email:
                    </label>
                    <input
                        type="text"
                        id="email"
                        value={email}
                        onChange={handleEmailChange}
                    />
                    <div className="error">{emailError}</div>
                </div>
                <button type="submit" className="submit-button">
                    Submit
                </button>
                {uploadSuccess && (
                    <div className="upload-success">File uploaded successfully!</div>
                )}
            </form>
        </div>
    );
};

export default Form;