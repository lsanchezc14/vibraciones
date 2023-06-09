from time import sleep
from threading import Thread
from queue import Queue

# import tflite_runtime.interpreter as tflite
import tensorflow as tf
import numpy as np
from opcua import Client

samples_number = 4
threshold = 0.01
path = 'C:\\Git\\ProyectoRaspberry\\ModelosEntrenados\\detector_anomalias.tflite'
url="opc.tcp://127.0.0.1:4840"

def sensor(queue):
    print('Inicia sensor')

    for i in range(samples_number):
        sample = np.array(np.random.rand(256,4096,3), dtype=np.float32)
        item = (i, sample)
        queue.put(item)

    queue.put(None)
    print('Sensor finaliza')
 
def consumer(queue):
    print('Inicia recoleccion')

    while True:

        item = queue.get()
        if item is None:
            break

        result = predict(item[1])
        if result == True:
            send_message()

        sleep(1)

    print('Finaliza recoleccion')
 
def predict(sample):
    #interpreter = tflite.Interpreter(model_path=path)
    interpreter = tf.lite.Interpreter(model_path=path)
    interpreter.allocate_tensors()
    input_details = interpreter.get_input_details()
    output_details = interpreter.get_output_details()
    interpreter.set_tensor(0, sample[0,:,:].reshape(1,4096,3))
    interpreter.invoke()
    output_data = interpreter.get_tensor(output_details[0]['index'])

    print('Anomaly detected?')
    result = output_data[0,0,0] > threshold
    print(result)

    return result

def send_message():
    client = Client(url)
    client.connect()
    print("Cliente conectado")
    node=client.get_node("ns=2;i=2")
    node.set_value(True)
    print('Mensaje enviado')


queue = Queue()

producer = Thread(target=sensor, args=(queue,))
producer.start()

consumer = Thread(target=consumer, args=(queue,))
consumer.start()

producer.join()
print("Finaliza el productor")
consumer.join()
print("Finaliza el consumidor")