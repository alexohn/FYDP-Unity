import tensorflow as tf, sys	

def classify_image():
	#image_path = sys.argv[1]
	image_path = "./cropped_ball.jpg"

	image_data = tf.gfile.FastGFile(image_path, 'rb').read()

	# Potentially change this line for the new path
	label_lines = [line.rstrip() for line in tf.gfile.GFile("./retrained_labels.txt")]

	with tf.gfile.FastGFile("./retrained_graph.pb", 'rb') as f:
		graph_def = tf.GraphDef()
		graph_def.ParseFromString(f.read())
		_ = tf.import_graph_def(graph_def, name='')

	with tf.Session() as sess:
		#Feed the image_data as input to the graph and get first prediciton
		softmax_tensor = sess.graph.get_tensor_by_name('final_result:0')

		predictions = sess.run(softmax_tensor, \
			{'DecodeJpeg/contents:0': image_data})

		#sort to show lavels of first prediction in order of confidence
		top_k = predictions[0].argsort()[-len(predictions[0]):][::-1]

		
		for node_id in top_k:
			human_string = label_lines[node_id]
			score = predictions[0][node_id]
			#print('%s (score = %.5f)' %(human_string, score))
			break
	
	return human_string + "-" + str(score)		

