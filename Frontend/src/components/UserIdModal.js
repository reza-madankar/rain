import React from 'react';
import { Modal, Form, Input, Button, Typography, message } from 'antd';
import { UserOutlined } from '@ant-design/icons';
import useUserStore from '../store/userStore';

const { Text } = Typography;

const UserIdModal = () => {
  const [form] = Form.useForm();
  const { userId, isFirstLoad, setUserId } = useUserStore();

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setUserId(values.userId);
      message.success('User ID saved successfully!');
    } catch (error) {
      message.error('Please enter a valid User ID');
    }
  };

  const handleCancel = () => {
    if (isFirstLoad) {
      message.warning('Please enter a User ID to continue');
      return;
    }
    form.resetFields();
  };

  return (
    <Modal
      title={
        <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
          <UserOutlined style={{ color: '#1890ff' }} />
          <span>Welcome to Authodesk Assignment</span>
        </div>
      }
      open={isFirstLoad || !userId}
      onCancel={handleCancel}
      footer={[
        <Button key="submit" type="primary" onClick={handleSubmit}>
          Continue
        </Button>
      ]}
      closable={!isFirstLoad}
      maskClosable={!isFirstLoad}
      width={500}
    >

      <Form
        form={form}
        layout="vertical"
        initialValues={{ userId: userId || '' }}
      >
        <Form.Item
          label="User ID"
          name="userId"
          rules={[
            {
              required: true,
              message: 'Please enter your User ID!',
            },
            {
              min: 1,
              message: 'User ID must be at least 1 character long!',
            },
          ]}
        >
          <Input
            placeholder="Enter your User ID"
            prefix={<UserOutlined />}
            size="large"
            autoFocus
          />
        </Form.Item>
      </Form>

      {isFirstLoad && (
        <div style={{
          marginTop: 16,
          padding: 12,
          backgroundColor: '#e6f7ff',
          border: '1px solid #91d5ff',
          borderRadius: 6,
          color: '#1890ff'
        }}>
          <Text>
            <strong>Note:</strong> This User ID will be used to authenticate all API requests.
          </Text>
        </div>
      )}
    </Modal>
  );
};

export default UserIdModal; 