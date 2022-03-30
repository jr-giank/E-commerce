import { Field, Form, Formik } from 'formik'
import React, { useContext, useEffect, useState } from 'react'
import { Navigate, useLocation, useNavigate } from 'react-router-dom'
import { AuthContext } from '../Context/AuthContext'
import { MenuCliente } from '../Menues/MenuCliente'
import * as Yup from 'yup';
import ItemGrid from './itemGrid'
import swal from 'sweetalert'
import { payBooks } from './PayService'

const PayPage = () => {
  
  const location = useLocation();
  const {user, state} = useContext(AuthContext)
  const [ totalCost, setTotalCost ] = useState(0)
  const [ cartDetails, setCartDetails ] = useState(null)
  const history = useNavigate();

  useEffect(()=>{
    
    let totalCost = 0;
    let cartDetails = []
    
    for (const iterator of state.cart) {
      
      totalCost += iterator.precio
      cartDetails = [...cartDetails, {
        idLibro: iterator.idLibro, cantidad: iterator.cantidad, monto: iterator.precio
      } ]
      
      setCartDetails([...cartDetails])
    }
    setTotalCost(totalCost)
    //eslint-disable-next-line
  }, [])
  
  const confirmPay = (paymentData)=>{
  
      swal({
        title: "Completar Pago",
        text: "¿Está seguro de que desea realizar el pago?",
        icon: "warning",
        buttons: ["No", "Si"]
      }).then(res => {
        if(res){
            
          payBooks(paymentData).then(message => {
              swal({
                  text: message,
                  icon: "success"
              });
              
              state.cart = [];
              history('/', {replace : true})

            }).catch(error => {
              swal({
                text : "Hubo un error al momento de procesar el pago. Intentelo más tarde",
                icon: "error"
              })
            })
        };
      });
  } 

  const handlePayment = (values) => {
    
    const paymentData = {
      nombreUsuario: user.data.nombreUsuario,
      numTarjeta: values.numeroTarjeta,
      cv: values.cvc,
      fechaVenc : `${values.mes}/${values.anio}`,
      carrito: cartDetails
    }
    
    confirmPay(paymentData)
  }

  if(user === null) {
      window.localStorage.setItem("lastPath", JSON.stringify(location.pathname))
      return <Navigate to='/login' replace />
  };

  if(state.cart.length === 0){
    return <Navigate to='/' replace />
  }

  return (
    <>
      <MenuCliente />
      <div className='flex flex-row flex-wrap bg-white overflow-auto h-full w-full justify-center'>
        <Formik initialValues={{pais:'', 
                                nombreTarjeta: '', 
                                numeroTarjeta: '', 
                                cvc:'', 
                                mes: '',
                                anio: ''   
                              }}
                              onSubmit={values => {
                                handlePayment(values)
                            }}
          >

            {({errors, touched}) => (

              <Form className='flex flex-col w-1/2 pb-10 mt-12'>
                <h1 className='mb-8 font-galdeano font-bold text-4xl'>Pagar</h1>
                <div className='flex flex-col w-full mt-4 h-16'>
                  <label htmlFor="pais">País de facturación</label>
                  <select
                    name='pais'
                    className='border outline-none w-6/12 h-10 mt-2'
                  >
                    <option value="República Dominicana">República Dominicana</option>
                    <option value="Estados Uniods">Estados Unidos</option>
                    <option value="Alemania">Alemania</option>
                  </select>
                </div>

                <div className='flex flex-row mt-4'>
                  <img 
                    src= {`../assets/AE.png`} 
                    alt=""
                    className='h-8 w-10 border rounded' 
                  />
                  <img 
                    src= {`../assets/discover.png`} 
                    alt=""
                    className='h-8 w-10 border rounded ml-1' 
                  />
                  <img 
                    src= {`../assets/visa.jpg`} 
                    alt=""
                    className='h-8 w-10 border rounded ml-1' 
                  />
                  <img 
                    src= {`../assets/mastercard.png`} 
                    alt=""
                    className='h-8 w-10 border rounded ml-1' 
                  />
                </div>

                <div className='border border-border-pay flex flex-row flex-wrap w-full mt-8 pb-12'>
                  <div className='w-full mt-4 flex justify-center h-10'>
                    <Field
                      name = 'nombreTarjeta'
                      placeholder = 'Nombre de la tarjeta'
                      className = 'border outline-none w-11/12 h-full pl-2'
                    >
                    </Field>
                  </div>

                  <div className='w-full mt-4 flex justify-center h-10'>
                    <Field
                      name = 'numeroTarjeta'
                      placeholder = 'Número de tarjeta'
                      className = 'border outline-none w-11/12 h-full pl-2'
                    >
                    </Field>
                  </div>

                  <div className='w-full mt-4 flex ml-6 h-24 flex-wrap'>
                    <Field
                      name = 'mes'
                      placeholder = 'Mes'
                      className = 'border outline-none w-5/12 h-10 pl-2'
                    >
                    </Field>
                    <Field
                      name = 'anio'
                      placeholder = 'Año'
                      className = 'border outline-none w-5/12 ml-14 h-10 pl-2'
                    >
                    </Field>
                    <Field
                      name = 'cvc'
                      placeholder = 'CVC'
                      className = 'border outline-none w-5/12 h-10 mt-4 pl-2'
                    >
                    </Field>
                  </div>
                <button type='submit' className='bg-orange text-white font-bold text-lg py-2 px-36 ml-32 mr-5 mt-8'>
                  Completar Pago
                </button>
                </div>
              </Form>
            )}
        </Formik>
          <div className='flex flex-col w-1/3 border border-border-pay h-64 ml-12 mt-12 shadow-md'>
            <div className='border-b mt-2 ml-4 mr-4 pb-2'>
              <h1 className='font-poppins font-bold text-2xl '>Resumen</h1>
            </div >
            <span className='flex flex-row mt-4 ml-4 mr-4 w-full'>
              <p className='font-poppins font-bold text-base text-left'>Total:</p>
              <p className='font-poppins text-base ml-4'> {totalCost} US$ </p>
            </span>
            
            <p className='text-xs mt-2 ml-5 mr-5'>
              Van Goh Library está obligado por ley a recaudar los impuestos sobre las transacciones de las compras realizadas en determinadas jurisdicciones fiscales.
            </p>

            <p className='text-xs mt-6 ml-5 mr-5'>
              Al completar la compra aceptas las condiciones de uso
            </p>
          </div>
          <div className='flex flex-col w-11/12 pb-10'>
            <div className='ml-9'>
             <h1 className='font-poppins text-2xl font-bold'>Detalles de la compra</h1>
                {state.cart.map(book => (
                  <ItemGrid
                    key={book.idLibro}
                    book = {book}
                  />
                )) 
                }
            </div>
          </div>
        </div>
    </>
  )
}
export default PayPage