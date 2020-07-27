export const Get = async (action, jwt, json) => {
    const response = await fetch(action,
        {
            method: 'GET',
            mode: 'cors',
            cache: 'no-cache',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json',
                'Jwt': jwt
            },

            redirect: 'follow',
            referrerPolicy: 'no-referrer',
        })

    if (response.ok)
        return await response.json();

    return { response: "Er is iets fout gegaan" };
}

export const Post = async (action, jwt, json) => {
    const response = await fetch(action,
        {
            method: 'POST',
            mode: 'cors',
            cache: 'no-cache',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json',
                'Jwt': jwt
            },

            redirect: 'follow',
            referrerPolicy: 'no-referrer',
            body: JSON.stringify(json)
        })

    if (response.ok)
        return await response.json();

    return { response: "Er is iets fout gegaan" };
}

export const Put = async (action, jwt, json) => {
    const response = await fetch(action,
        {
            method: 'PUT',
            mode: 'cors',
            cache: 'no-cache',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json',
                'Jwt': jwt
            },

            redirect: 'follow',
            referrerPolicy: 'no-referrer',
            body: JSON.stringify(json)
        })

    if (response.ok)
        return await response.json();

    return { response: "Er is iets fout gegaan" };
}

export const Delete = async function (action, jwt, json) {
    const response = await fetch(action,
        {
            method: 'DELETE',
            mode: 'cors',
            cache: 'no-cache',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json',
                'Jwt': jwt
            },

            redirect: 'follow',
            referrerPolicy: 'no-referrer',
            body: JSON.stringify(json)
        })

    if (response.ok)
        return await response.json();

    return { response: "Er is iets fout gegaan" };
}