import React, { useState } from 'react';
import Button from "@mui/material/button"


const Form = () => {
    const [selectedFile, setSelectedFile] = useState(null);
    const [email, setEmail] = useState('');
    const [fileError, setFileError] = useState('');
    const [emailError, setEmailError] = useState('');

    const handleFileChange = (event) => {
        const file = event.target.files[0];
        if (file && file.name.endsWith('.docx')) {
            setSelectedFile(file);
            setFileError('');
        } else {
            setSelectedFile(null);
            setFileError('Please select a .docx file.');
        }
    };

    const handleEmailChange = (event) => {
        setEmail(event.target.value);
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
        if (!emailPattern.test(email)) {
            setEmailError('Please enter a valid email address.');
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
                } else {
                    console.error('File upload failed.');
                }
            }
        } catch (error) {
            console.error('An error occurred during file upload:', error);
        }
    };

    return (
        <div className="root">
            <h2>Upload a .docx File</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>Upload .docx File:</label>
                    <input type="file" accept=".docx" onChange={handleFileChange} />
                    <div className="error">{fileError}</div>
                </div>
                <div>
                    <label>Email:</label>
                    <input type="text" value={email} onChange={handleEmailChange} />
                    <div className="error">{emailError}</div>
                </div>
                <Button variant="contained" color="primary" type="submit">
                    Submit
                </Button>
                {/*<button type="submit">Submit</button>*/}
            </form>
        </div>
    );
};

export default Form;