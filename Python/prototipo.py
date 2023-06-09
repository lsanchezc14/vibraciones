import tflite_runtime.interpreter as tflite

threshold = 0.1

test = np.array(np.random.rand(256,4096,3), dtype=np.float32)

interpreter = tflite.Interpreter(model_path=args.model_file)

interpreter.allocate_tensors()

input_details = interpreter.get_input_details()
output_details = interpreter.get_output_details()

interpreter.set_tensor(0, test[0,:,:].reshape(1,4096,3))

interpreter.invoke()

output_data = interpreter.get_tensor(output_details[0]['index'])

print('Anomaly detected?')
print(output_data[0,0,0] > threshold)