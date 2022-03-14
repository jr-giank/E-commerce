const API = 'https://localhost:44373/api/Libro';

export const registerService = async ({values}) => {    
    
    let form = new FormData();

    form.append('Nombre', values.Nombre);
    form.append('Precio', values.Precio);
    form.append('Autor', values.Autor);
    form.append('Año', values.Año);
    form.append('Editorial', values.Editorial);
    form.append('NumeroPaginas', values.NumeroPaginas);
    form.append('Idioma', values.Idioma);
    form.append('IdCategoria', values.IdCategoria);
    form.append('Foto', values.Foto);

    let response = await fetch(API, {

        method: 'POST',
        body: form
    });

    let data = await response.json();

    if (response.status === 200){
        return data
    }
    
} 