import React, { useState } from 'react';
import { 
  Modal, 
  Form, 
  Switch, 
  Button, 
  message, 
  Typography 
} from 'antd';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import axios from '../helper/axios';
import useUserStore from '../store/userStore';

const { Title } = Typography;

const AddRainModal = ({ visible, onCancel, onSuccess }) => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const queryClient = useQueryClient();
  const { userId } = useUserStore();

  const addRainMutation = useMutation({
    mutationFn: async (data) => {
      const response = await axios.post('/data', { rain: data.rain });
      return response.data;
    },
    onSuccess: (data) => {
      message.success('Rain record added successfully!');
      form.resetFields();
      onSuccess();
      queryClient.invalidateQueries({ queryKey: ['rainData'] });
    },
    onError: (error) => {
      const errorMessage = error.response?.data?.description || 
                          error.response?.data?.message || 
                          error.message || 
                          'Failed to add rain record';
      message.error(errorMessage);
    }
  });

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      await addRainMutation.mutateAsync(values);
    } catch (error) {
      if (error.errorFields) {
        message.error('Please check the form and try again.');
      }
    } finally {
      setLoading(false);
    }
  };

  const handleCancel = () => {
    form.resetFields();
    onCancel();
  };

  return (
    <Modal
      title="Add New Rain Record"
      open={visible}
      onCancel={handleCancel}
      footer={[
        <Button key="cancel" onClick={handleCancel}>
          Cancel
        </Button>,
        <Button 
          key="submit" 
          type="primary" 
          loading={loading}
          onClick={handleSubmit}
        >
          Add Record
        </Button>
      ]}
      width={500}
    >
      <Form
        form={form}
        layout="vertical"
        initialValues={{ rain: true }}
      >
        <Form.Item
          label="Rain Status"
          name="rain"
          rules={[
            {
              required: true,
              message: 'Please select rain status!',
            },
          ]}
        >
          <Switch
            checkedChildren="Rain"
            unCheckedChildren="No Rain"
            defaultChecked
          />
        </Form.Item>

        <div style={{ marginTop: 16 }}>
          <Title level={5}>Current User ID: {userId}</Title>
        </div>

        {addRainMutation.error && (
          <div style={{ 
            marginTop: 16, 
            padding: 12, 
            backgroundColor: '#fff2f0', 
            border: '1px solid #ffccc7',
            borderRadius: 6,
            color: '#cf1322'
          }}>
            <strong>Error:</strong> {addRainMutation.error.response?.data?.description || 
                                   addRainMutation.error.message || 
                                   'An error occurred'}
          </div>
        )}
      </Form>
    </Modal>
  );
};

export default AddRainModal; 